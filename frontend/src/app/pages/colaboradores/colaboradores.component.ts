import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ColaboradorService } from '../../services/colaborador.service';
import { UnidadeService } from '../../services/unidade.service';
import { UsuarioService } from '../../services/usuario.service';
import { Colaborador, CriarColaborador, AtualizarColaborador } from '../../models/colaborador.model';
import { Unidade } from '../../models/unidade.model';
import { Usuario } from '../../models/usuario.model';

@Component({
  selector: 'app-colaboradores',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="page-header">
        <h2>Colaboradores</h2>
        <button class="btn btn-primary" (click)="openCreate()">Novo Colaborador</button>
      </div>

      <div class="alert alert-success" *ngIf="successMsg">{{ successMsg }}</div>
      <div class="alert alert-error" *ngIf="errorMsg">{{ errorMsg }}</div>

      <table>
        <thead>
          <tr>
            <th>Código</th>
            <th>Nome</th>
            <th>Unidade</th>
            <th>Usuário</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let col of colaboradores">
            <td>{{ col.codigo }}</td>
            <td>{{ col.nome }}</td>
            <td>{{ col.unidadeNome }}</td>
            <td>{{ col.usuarioLogin }}</td>
            <td>
              <div class="actions">
                <button class="btn btn-warning" (click)="openEdit(col)">Editar</button>
                <button class="btn btn-danger" (click)="remover(col.id)">Remover</button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>

      <!-- Modal Criar -->
      <div class="modal-overlay" *ngIf="showModal" (click)="showModal = false">
        <div class="modal" (click)="$event.stopPropagation()">
          <h3>Novo Colaborador</h3>
          <div class="form-group">
            <label>Nome</label>
            <input type="text" [(ngModel)]="novoColaborador.nome">
          </div>
          <div class="form-group">
            <label>Unidade</label>
            <select [(ngModel)]="novoColaborador.unidadeId">
              <option [ngValue]="0" disabled>Selecione...</option>
              <option *ngFor="let u of unidades" [ngValue]="u.id">{{ u.nome }} ({{ u.codigo }})</option>
            </select>
          </div>
          <div class="form-group">
            <label>Usuário</label>
            <select [(ngModel)]="novoColaborador.usuarioId">
              <option [ngValue]="0" disabled>Selecione...</option>
              <option *ngFor="let u of usuarios" [ngValue]="u.id">{{ u.login }}</option>
            </select>
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
          <h3>Editar Colaborador</h3>
          <div class="form-group">
            <label>Nome</label>
            <input type="text" [(ngModel)]="editData.nome">
          </div>
          <div class="form-group">
            <label>Unidade</label>
            <select [(ngModel)]="editData.unidadeId">
              <option *ngFor="let u of unidades" [ngValue]="u.id">{{ u.nome }} ({{ u.codigo }})</option>
            </select>
          </div>
          <div class="modal-actions">
            <button class="btn" (click)="showEditModal = false">Cancelar</button>
            <button class="btn btn-primary" (click)="atualizar()">Salvar</button>
          </div>
        </div>
      </div>
    </div>
  `
})
export class ColaboradoresComponent implements OnInit {
  colaboradores: Colaborador[] = [];
  unidades: Unidade[] = [];
  usuarios: Usuario[] = [];
  showModal = false;
  showEditModal = false;
  novoColaborador: CriarColaborador = { nome: '', unidadeId: 0, usuarioId: 0 };
  editData: AtualizarColaborador & { id?: number } = {};
  successMsg = '';
  errorMsg = '';

  constructor(
    private colaboradorService: ColaboradorService,
    private unidadeService: UnidadeService,
    private usuarioService: UsuarioService
  ) {}

  ngOnInit(): void {
    this.loadColaboradores();
  }

  loadColaboradores(): void {
    this.colaboradorService.getAll().subscribe({
      next: (data) => this.colaboradores = data,
      error: () => this.errorMsg = 'Erro ao carregar colaboradores.'
    });
  }

  openCreate(): void {
    this.unidadeService.getAll().subscribe(u => this.unidades = u.filter(x => x.ativa));
    this.usuarioService.getAll().subscribe(u => this.usuarios = u.filter(x => x.ativo));
    this.novoColaborador = { nome: '', unidadeId: 0, usuarioId: 0 };
    this.showModal = true;
  }

  openEdit(col: Colaborador): void {
    this.unidadeService.getAll().subscribe(u => this.unidades = u.filter(x => x.ativa));
    this.editData = { id: col.id, nome: col.nome, unidadeId: col.unidadeId };
    this.showEditModal = true;
  }

  criar(): void {
    this.colaboradorService.create(this.novoColaborador).subscribe({
      next: () => {
        this.showModal = false;
        this.successMsg = 'Colaborador criado com sucesso!';
        this.loadColaboradores();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao criar colaborador.';
        this.clearMessages();
      }
    });
  }

  atualizar(): void {
    if (!this.editData.id) return;
    const dto: AtualizarColaborador = {
      nome: this.editData.nome,
      unidadeId: this.editData.unidadeId
    };

    this.colaboradorService.update(this.editData.id, dto).subscribe({
      next: () => {
        this.showEditModal = false;
        this.successMsg = 'Colaborador atualizado com sucesso!';
        this.loadColaboradores();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao atualizar colaborador.';
        this.clearMessages();
      }
    });
  }

  remover(id: number): void {
    if (!confirm('Deseja realmente remover este colaborador?')) return;

    this.colaboradorService.delete(id).subscribe({
      next: () => {
        this.successMsg = 'Colaborador removido com sucesso!';
        this.loadColaboradores();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao remover colaborador.';
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
