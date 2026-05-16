import { Injectable, signal, PLATFORM_ID, Inject, NgZone } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';

export interface AuthUser {
  name: string;
  email: string;
  picture: string;
  sub: string; // Provider user ID
  provider: 'google' | 'facebook' | 'local';
}

// Extend Window to include google GSI
declare global {
  interface Window {
    google?: {
      accounts: {
        id: {
          initialize: (config: {
            client_id: string;
            callback: (response: { credential: string }) => void;
            auto_select?: boolean;
            cancel_on_tap_outside?: boolean;
          }) => void;
          renderButton: (
            parent: HTMLElement,
            options: {
              theme?: string;
              size?: string;
              text?: string;
              shape?: string;
              logo_alignment?: string;
              width?: number;
            }
          ) => void;
          prompt: () => void;
          disableAutoSelect: () => void;
          revoke: (hint: string, callback: () => void) => void;
        };
      };
    };
  }
}

// Facebook SDK type declarations
declare const FB: {
  init: (params: {
    appId: string;
    cookie?: boolean;
    xfbml?: boolean;
    version: string;
  }) => void;
  login: (
    callback: (response: {
      authResponse?: {
        accessToken: string;
        userID: string;
      };
      status: string;
    }) => void,
    options?: { scope: string }
  ) => void;
  api: (
    path: string,
    params: { fields: string },
    callback: (response: {
      id: string;
      name: string;
      email?: string;
      picture?: { data?: { url?: string } };
    }) => void
  ) => void;
  getLoginStatus: (
    callback: (response: { status: string }) => void
  ) => void;
};

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  // ──────────────────────────────────────────────────────────
  // 🔑 REPLACE THIS WITH YOUR GOOGLE CLIENT ID
  // Get it from: https://console.cloud.google.com/
  // ──────────────────────────────────────────────────────────
  private readonly CLIENT_ID =
    '158844941712-ur7qfnon9j69tg1vkobfdbip5a4ujm0e.apps.googleusercontent.com';

  // ──────────────────────────────────────────────────────────
  // 🔑 REPLACE THIS WITH YOUR FACEBOOK APP ID
  // Get it from: https://developers.facebook.com/
  // ──────────────────────────────────────────────────────────
  private readonly FB_APP_ID = '2163399284518310';

  private readonly STORAGE_KEY = 'gavelcore-auth-user';

  // Reactive state
  currentUser = signal<AuthUser | null>(null);
  isAuthenticated = signal<boolean>(false);

  constructor(
    private router: Router,
    private ngZone: NgZone,
    @Inject(PLATFORM_ID) private platformId: object
  ) {
    this.restoreSession();
  }

  /** Restore user from localStorage on page reload */
  private restoreSession(): void {
    if (!isPlatformBrowser(this.platformId)) return;
    try {
      const stored = localStorage.getItem(this.STORAGE_KEY);
      if (stored) {
        const user: AuthUser = JSON.parse(stored);
        this.currentUser.set(user);
        this.isAuthenticated.set(true);
      }
    } catch {
      this.clearSession();
    }
  }

  // ═══════════════════════════════════════════════════════
  // GOOGLE SIGN-IN
  // ═══════════════════════════════════════════════════════

  /**
   * Initialize the Google GSI SDK.
   * Call this from the login component after view init.
   * @param buttonContainer – the DOM element where the Google button will render
   */
  initializeGoogleSignIn(buttonContainer: HTMLElement): void {
    if (!isPlatformBrowser(this.platformId)) return;

    const tryInit = () => {
      if (window.google?.accounts?.id) {
        window.google.accounts.id.initialize({
          client_id: this.CLIENT_ID,
          callback: (response) => this.handleCredentialResponse(response),
          cancel_on_tap_outside: true,
        });

        window.google.accounts.id.renderButton(buttonContainer, {
          theme: 'outline',
          size: 'large',
          text: 'signin_with',
          shape: 'rectangular',
          logo_alignment: 'left',
          width: buttonContainer.offsetWidth > 200 ? buttonContainer.offsetWidth : 300,
        });
      } else {
        // GSI script not yet loaded – retry
        setTimeout(tryInit, 300);
      }
    };

    tryInit();
  }

  /** Called by GSI with the JWT credential after user selects a Google account */
  private handleCredentialResponse(response: { credential: string }): void {
    try {
      const payload = this.decodeJwt(response.credential);
      const user: AuthUser = {
        name: payload['name'] as string,
        email: payload['email'] as string,
        picture: payload['picture'] as string,
        sub: payload['sub'] as string,
        provider: 'google',
      };

      this.currentUser.set(user);
      this.isAuthenticated.set(true);

      if (isPlatformBrowser(this.platformId)) {
        localStorage.setItem(this.STORAGE_KEY, JSON.stringify(user));
        // Store raw JWT if you need to send it to a backend
        localStorage.setItem('gavelcore-google-token', response.credential);
      }

      this.router.navigate(['/home']);
    } catch (err) {
      console.error('Error al procesar el inicio de sesión con Google:', err);
    }
  }

  /** Decode a JWT without validation (client-side only) */
  private decodeJwt(token: string): Record<string, unknown> {
    const base64 = token.split('.')[1];
    const padded = base64.replace(/-/g, '+').replace(/_/g, '/');
    const json = decodeURIComponent(
      atob(padded)
        .split('')
        .map((c) => '%' + c.charCodeAt(0).toString(16).padStart(2, '0'))
        .join('')
    );
    return JSON.parse(json);
  }

  // ═══════════════════════════════════════════════════════
  // FACEBOOK SIGN-IN
  // ═══════════════════════════════════════════════════════

  /** Initialize the Facebook SDK (call once on app init) */
  initializeFacebookSDK(): void {
    if (!isPlatformBrowser(this.platformId)) return;

    const tryInit = () => {
      if (typeof (window as any).FB !== 'undefined') {
        (window as any).FB.init({
          appId: this.FB_APP_ID,
          cookie: true,
          xfbml: true,
          version: 'v19.0',
        });
      } else {
        setTimeout(tryInit, 300);
      }
    };

    tryInit();
  }

  /** Open Facebook login dialog and handle the response */
  loginWithFacebook(onDone?: () => void): void {
    if (!isPlatformBrowser(this.platformId)) return;

    const tryLogin = () => {
      const fb = (window as any).FB;
      if (fb) {
        fb.login(
          (response: any) => {
            if (response.authResponse) {
              this.fetchFacebookUser(response.authResponse.accessToken, onDone);
            } else {
              console.warn('Facebook login cancelled by user.');
              if (onDone) onDone();
            }
          },
          { scope: 'public_profile,email' }
        );
      } else {
        setTimeout(tryLogin, 300);
      }
    };

    tryLogin();
  }

  /** Fetch Facebook user profile data via Graph API */
  private fetchFacebookUser(accessToken: string, onDone?: () => void): void {
    const fb = (window as any).FB;
    fb.api('/me', { fields: 'id,name,email,picture.width(200)' }, (fbUser: any) => {
      // FB callbacks run outside Angular zone — re-enter to trigger change detection
      this.ngZone.run(() => {
        const user: AuthUser = {
          name: fbUser.name,
          email: fbUser.email || `${fbUser.id}@facebook.com`,
          picture: fbUser.picture?.data?.url || '',
          sub: fbUser.id,
          provider: 'facebook',
        };

        this.currentUser.set(user);
        this.isAuthenticated.set(true);

        if (isPlatformBrowser(this.platformId)) {
          localStorage.setItem(this.STORAGE_KEY, JSON.stringify(user));
          localStorage.setItem('gavelcore-fb-token', accessToken);
        }

        if (onDone) onDone();
        this.router.navigate(['/home']);
      });
    });
  }

  // ═══════════════════════════════════════════════════════
  // LOCAL CREDENTIALS
  // ═══════════════════════════════════════════════════════

  /**
   * Authenticate with local credentials (username/password).
   * Stores a local session so the authGuard lets the user through.
   */
  loginWithCredentials(username: string): void {
    const user: AuthUser = {
      name: username,
      email: username.includes('@') ? username : `${username}@local`,
      picture: '',
      sub: 'local-' + username,
      provider: 'local',
    };
    this.currentUser.set(user);
    this.isAuthenticated.set(true);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem(this.STORAGE_KEY, JSON.stringify(user));
    }
  }

  // ═══════════════════════════════════════════════════════
  // SIGN OUT
  // ═══════════════════════════════════════════════════════

  /** Sign out the current user */
  signOut(): void {
    const user = this.currentUser();

    // Revoke Google session if applicable
    if (
      isPlatformBrowser(this.platformId) &&
      window.google?.accounts?.id &&
      user?.provider === 'google'
    ) {
      if (user.email) {
        window.google.accounts.id.revoke(user.email, () => {});
      }
      window.google.accounts.id.disableAutoSelect();
    }

    // Logout from Facebook if applicable
    const fb = (window as any).FB;
    if (isPlatformBrowser(this.platformId) && fb && user?.provider === 'facebook') {
      fb.getLoginStatus((response: any) => {
        if (response.status === 'connected') {
          fb.logout(() => {});
        }
      });
    }

    this.clearSession();
    this.router.navigate(['/']);
  }

  private clearSession(): void {
    this.currentUser.set(null);
    this.isAuthenticated.set(false);
    if (isPlatformBrowser(this.platformId)) {
      localStorage.removeItem(this.STORAGE_KEY);
      localStorage.removeItem('gavelcore-google-token');
      localStorage.removeItem('gavelcore-fb-token');
    }
  }
}
