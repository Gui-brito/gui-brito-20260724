import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsuarioService } from '../../services/usuario.service';
import { Usuario, CriarUsuario, AtualizarUsuario } from '../../models/usuario.model';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="page-header">
        <h2>Usuários</h2>
        <div class="actions">
          <select [(ngModel)]="filtroStatus" (change)="loadUsuarios()" class="filter-select">
            <option value="">Todos</option>
            <option value="true">Ativos</option>
            <option value="false">Inativos</option>
          </select>
          <button class="btn btn-primary" (click)="showModal = true">Novo Usuário</button>
        </div>
      </div>

      <div class="alert alert-success" *ngIf="successMsg">{{ successMsg }}</div>
      <div class="alert alert-error" *ngIf="errorMsg">{{ errorMsg }}</div>

      <table>
        <thead>
          <tr>
            <th>Código</th>
            <th>Login</th>
            <th>Status</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let usuario of usuarios">
            <td>{{ usuario.codigo }}</td>
            <td>{{ usuario.login }}</td>
            <td>
              <span class="badge" [class.badge-active]="usuario.ativo" [class.badge-inactive]="!usuario.ativo">
                {{ usuario.ativo ? 'Ativo' : 'Inativo' }}
              </span>
            </td>
            <td>
              <div class="actions">
                <button class="btn btn-warning" (click)="openEdit(usuario)">Editar</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Modal Criar -->
      <div class="modal-overlay" *ngIf="showModal" (click)="showModal = false">
        <div class="modal" (click)="$event.stopPropagation()">
          <h3>Novo Usuário</h3>
          <div class="form-group">
            <label>Login</label>
            <input type="text" [(ngModel)]="novoUsuario.login">
          </div>
          <div class="form-group">
            <label>Senha</label>
            <input type="password" [(ngModel)]="novoUsuario.senha">
          </div>
          <div class="modal-actions">
            <button class="btn" (click)="showModal = false">Cancelar</button>
            <button class="btn btn-primary" (click)="criar()">Criar</button>
          </div>
        </div>
      </div>

      <!-- Modal Editar -->
      <div class="modal-overlay" *ngIf="showEditModal" (click)="showEditModal = false">
        <div class="modal" (click)="$event.stopPropagation()">
          <h3>Editar Usuário</h3>
          <div class="form-group">
            <label>Nova Senha (deixe vazio para manter)</label>
            <input type="password" [(ngModel)]="editData.senha">
          </div>
          <div class="form-group">
            <label>Status</label>
            <select [(ngModel)]="editData.ativo">
              <option [ngValue]="true">Ativo</option>
              <option [ngValue]="false">Inativo</option>
            </select>
          </div>
          <div class="modal-actions">
            <button class="btn" (click)="showEditModal = false">Cancelar</button>
            <button class="btn btn-primary" (click)="atualizar()">Salvar</button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .filter-select {
      padding: 8px 12px;
      border: 1px solid #ccc;
      border-radius: 4px;
      font-size: 14px;
    }
  `]
})
export class UsuariosComponent implements OnInit {
  usuarios: Usuario[] = [];
  showModal = false;
  showEditModal = false;
  filtroStatus = '';
  novoUsuario: CriarUsuario = { login: '', senha: '' };
  editData: AtualizarUsuario & { id?: number } = {};
  successMsg = '';
  errorMsg = '';

  constructor(private usuarioService: UsuarioService) {}

  ngOnInit(): void {
    this.loadUsuarios();
  }

  loadUsuarios(): void {
    const ativo = this.filtroStatus === '' ? undefined : this.filtroStatus === 'true';
    this.usuarioService.getAll(ativo).subscribe({
      next: (data) => this.usuarios = data,
      error: () => this.errorMsg = 'Erro ao carregar usuários.'
    });
  }

  criar(): void {
    this.usuarioService.create(this.novoUsuario).subscribe({
      next: () => {
        this.showModal = false;
        this.novoUsuario = { login: '', senha: '' };
        this.successMsg = 'Usuário criado com sucesso!';
        this.loadUsuarios();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao criar usuário.';
        this.clearMessages();
      }
    });
  }

  openEdit(usuario: Usuario): void {
    this.editData = { id: usuario.id, ativo: usuario.ativo, senha: '' };
    this.showEditModal = true;
  }

  atualizar(): void {
    if (!this.editData.id) return;
    const dto: AtualizarUsuario = { ativo: this.editData.ativo };
    if (this.editData.senha) dto.senha = this.editData.senha;

    this.usuarioService.update(this.editData.id, dto).subscribe({
      next: () => {
        this.showEditModal = false;
        this.successMsg = 'Usuário atualizado com sucesso!';
        this.loadUsuarios();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao atualizar usuário.';
        this.clearMessages();
      }
    });
  }

  private clearMessages(): void {
    setTimeout(() => {
      this.successMsg = '';
      this.errorMsg = '';
    }, 3000);
  }
}
