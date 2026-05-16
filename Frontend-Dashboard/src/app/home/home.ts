import { CommonModule, DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

interface MenuItem {
  key: string;
  label: string;
  active?: boolean;
}

interface StatCard {
  title: string;
  value: string;
  subtitle: string;
}

import { AgendaComponent } from '../agenda/agenda';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, AgendaComponent],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class HomeComponent implements OnInit {
  sidebarCollapsed = false;
  isDarkMode = false;
  showUserMenu = false;
  currentView = 'dashboard';

  // Get user data from signal safely using inject()
  private authService = inject(AuthService);
  user = this.authService.currentUser;

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private router: Router
  ) {}

  ngOnInit(): void {
    const savedTheme = localStorage.getItem('gavelcore-theme');
    if (savedTheme === 'dark') {
      this.isDarkMode = true;
      this.document.body.classList.add('dark-theme');
    }
  }

  toggleTheme(): void {
    this.isDarkMode = !this.isDarkMode;
    if (this.isDarkMode) {
      this.document.body.classList.add('dark-theme');
      localStorage.setItem('gavelcore-theme', 'dark');
    } else {
      this.document.body.classList.remove('dark-theme');
      localStorage.setItem('gavelcore-theme', 'light');
    }
  }

  menuGroups = [
    {
      title: 'GESTIÓN JURÍDICA',
      items: [
        { label: 'Panel de Control', key: 'dashboard', active: true },
        { label: 'Expedientes Judiciales', key: 'casos', active: false },
        { label: 'Agenda y Audiencias', key: 'calendario', active: false },
        { label: 'Base de Clientes', key: 'clientes', active: false },
      ]
    },
    {
      title: 'DOCUMENTACIÓN Y FIRMA',
      items: [
        { label: 'Biblioteca de Modelos', key: 'documentos', active: false },
        { label: 'Firmas Digitales', key: 'tareas', active: false },
      ]
    },
    {
      title: 'ADMINISTRACIÓN',
      items: [
        { label: 'Honorarios y Pagos', key: 'facturacion', active: false },
        { label: 'Consultas Online', key: 'chat', active: false },
      ]
    }
  ];

  stats: StatCard[] = [
    { title: 'Expedientes Activos', value: '24', subtitle: 'casos' },
    { title: 'Audiencias Próximas', value: '3', subtitle: 'audiencias' },
    { title: 'Plazos por Vencer', value: '8', subtitle: 'plazos' },
    { title: 'Facturación Mensual', value: '$12,450', subtitle: 'money' }
  ];

  quickAccess: unknown[] = [];
  activity: unknown[] = [];
  recentDocuments: unknown[] = [];
  pendingTasks: unknown[] = [];

  setActive(selectedItem: any): void {
    this.menuGroups.forEach(group => {
      group.items.forEach(item => {
        item.active = (item === selectedItem);
      });
    });
    this.currentView = selectedItem.key;
  }

  logout(): void {
    this.authService.signOut();
  }

  toggleUserMenu(): void {
    this.showUserMenu = !this.showUserMenu;
  }

  toggleSidebar(): void {
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }
}