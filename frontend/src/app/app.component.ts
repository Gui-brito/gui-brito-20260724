import { Component } from '@angular/core';
import { RouterOutlet, Router, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  template: `
    <div class="app-layout">
      <nav class="sidebar" *ngIf="authService.isLoggedIn()">
        <div class="sidebar-header">
          <h1>Gestão</h1>
        </div>
        <ul class="nav-links">
          <li>
            <a routerLink="/dashboard/usuarios" routerLinkActive="active">Usuários</a>
          </li>
          <li>
            <a routerLink="/dashboard/colaboradores" routerLinkActive="active">Colaboradores</a>
          </li>
          <li>
            <a routerLink="/dashboard/unidades" routerLinkActive="active">Unidades</a>
          </li>
        </ul>
        <div class="sidebar-footer">
          <button class="btn btn-danger" (click)="logout()">Sair</button>
        </div>
      </nav>
      <main [class.with-sidebar]="authService.isLoggedIn()">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styles: [`
    .app-layout {
      display: flex;
      min-height: 100vh;
    }

    .sidebar {
      width: 240px;
      background: #1976d2;
      color: white;
      display: flex;
      flex-direction: column;
      position: fixed;
      height: 100vh;
    }

    .sidebar-header {
      padding: 24px;
      border-bottom: 1px solid rgba(255,255,255,0.1);
    }

    .sidebar-header h1 {
      font-size: 20px;
    }

    .nav-links {
      list-style: none;
      padding: 16px 0;
      flex: 1;
    }

    .nav-links li a {
      display: block;
      padding: 12px 24px;
      color: rgba(255,255,255,0.8);
      text-decoration: none;
      transition: all 0.3s;
    }

    .nav-links li a:hover,
    .nav-links li a.active {
      background: rgba(255,255,255,0.1);
      color: white;
    }

    .sidebar-footer {
      padding: 16px 24px;
      border-top: 1px solid rgba(255,255,255,0.1);
    }

    .sidebar-footer .btn {
      width: 100%;
    }

    main {
      flex: 1;
      padding: 24px;
    }

    main.with-sidebar {
      margin-left: 240px;
    }
  `]
})
export class AppComponent {
  constructor(
    public authService: AuthService,
    private router: Router
  ) {}

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
