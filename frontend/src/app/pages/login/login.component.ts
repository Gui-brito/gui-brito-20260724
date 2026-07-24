import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="login-container">
      <div class="login-card">
        <h2>Gestão de Colaboradores</h2>
        <p class="subtitle">Faça login para continuar</p>

        <div class="alert alert-error" *ngIf="error">{{ error }}</div>

        <form (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label for="login">Login</label>
            <input type="text" id="login" [(ngModel)]="credentials.login" name="login" required>
          </div>

          <div class="form-group">
            <label for="senha">Senha</label>
            <input type="password" id="senha" [(ngModel)]="credentials.senha" name="senha" required>
          </div>

          <button type="submit" class="btn btn-primary btn-block" [disabled]="loading">
            {{ loading ? 'Entrando...' : 'Entrar' }}
          </button>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
      display: flex;
      align-items: center;
      justify-content: center;
      min-height: 100vh;
      background: linear-gradient(135deg, #1976d2, #1565c0);
    }

    .login-card {
      background: white;
      padding: 40px;
      border-radius: 8px;
      box-shadow: 0 8px 32px rgba(0,0,0,0.2);
      width: 100%;
      max-width: 400px;
    }

    .login-card h2 {
      color: #1976d2;
      margin-bottom: 8px;
      text-align: center;
    }

    .subtitle {
      color: #666;
      text-align: center;
      margin-bottom: 24px;
    }

    .btn-block {
      width: 100%;
      padding: 12px;
      font-size: 16px;
    }
  `]
})
export class LoginComponent {
  credentials = { login: '', senha: '' };
  error = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    this.loading = true;
    this.error = '';

    this.authService.login(this.credentials).subscribe({
      next: () => {
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.error = err.error?.message || 'Erro ao realizar login.';
        this.loading = false;
      }
    });
  }
}
