import { CommonModule, DOCUMENT, isPlatformBrowser } from '@angular/common';
import {
  AfterViewInit,
  Component,
  ElementRef,
  Inject,
  OnInit,
  PLATFORM_ID,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent implements OnInit, AfterViewInit {
  @ViewChild('googleBtn') googleBtnRef!: ElementRef<HTMLDivElement>;

  email = '';
  password = '';
  showPassword = false;
  rememberMe = false;
  errorMessage = '';
  isLoading = false;
  isDarkMode = false;
  currentView: 'login' | 'register' | 'forgot' = 'login';
  registerPassword = '';

  getPasswordStrength() {
    const p = this.registerPassword;
    if (!p) return { score: 0, label: '', color: '' };

    let score = 0;
    if (p.length > 5) score++;
    if (p.length > 8) score++;
    if (/[A-Z]/.test(p)) score++;
    if (/[0-9]/.test(p)) score++;
    if (/[^A-Za-z0-9]/.test(p)) score++;

    if (score <= 2) return { score: 33, label: 'Débil', color: '#ff4d4d' };
    if (score <= 4) return { score: 66, label: 'Media', color: '#ffcc00' };
    return { score: 100, label: 'Fuerte', color: '#00cc66' };
  }

  setView(view: 'login' | 'register' | 'forgot'): void {
    this.currentView = view;
    this.errorMessage = '';
    if (view === 'login') {
      setTimeout(() => this.renderGoogleButton(), 50);
    }
  }

  facebookLoading = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    @Inject(DOCUMENT) private document: Document,
    @Inject(PLATFORM_ID) private platformId: object
  ) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/home']);
      return;
    }

    if (isPlatformBrowser(this.platformId)) {
      const savedTheme = localStorage.getItem('gavelcore-theme');
      if (savedTheme === 'dark') {
        this.isDarkMode = true;
        this.document.body.classList.add('dark-theme');
      }
    }
  }

  ngAfterViewInit(): void {
    this.renderGoogleButton();
    this.authService.initializeFacebookSDK();
  }

  private renderGoogleButton(): void {
    if (isPlatformBrowser(this.platformId) && this.googleBtnRef?.nativeElement) {
      this.authService.initializeGoogleSignIn(this.googleBtnRef.nativeElement);
    }
  }

  loginWithFacebook(): void {
    this.facebookLoading = true;
    this.authService.loginWithFacebook(() => {
      this.facebookLoading = false;
    });
  }

  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      this.document.body.classList.add('dark-theme');
      if (isPlatformBrowser(this.platformId)) {
        localStorage.setItem('gavelcore-theme', 'dark');
      }
    } else {
      this.document.body.classList.remove('dark-theme');
      if (isPlatformBrowser(this.platformId)) {
        localStorage.setItem('gavelcore-theme', 'light');
      }
    }
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    const USERNAME = 'admin123';
    const EMAIL = 'admin@gavelcore.com';
    const PASS = '123';

    if (
      (this.email === USERNAME || this.email === EMAIL) &&
      this.password === PASS
    ) {
      this.errorMessage = '';
      this.isLoading = true;
      this.authService.loginWithCredentials(this.email);
      setTimeout(() => {
        this.router.navigate(['/home']);
      }, 2200);
    } else {
      this.errorMessage = 'Credenciales incorrectas';
    }
  }
}