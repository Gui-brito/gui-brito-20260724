import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UnidadeService } from '../../services/unidade.service';
import { Unidade, CriarUnidade } from '../../models/unidade.model';

@Component({
  selector: 'app-unidades',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="container">
      <div class="page-header">
        <h2>Unidades</h2>
        <button class="btn btn-primary" (click)="showModal = true">Nova Unidade</button>
      </div>

      <div class="alert alert-success" *ngIf="successMsg">{{ successMsg }}</div>
      <div class="alert alert-error" *ngIf="errorMsg">{{ errorMsg }}</div>

      <div class="card" *ngFor="let unidade of unidades">
        <div class="unidade-header">
          <div>
            <h3>{{ unidade.nome }}</h3>
            <p class="unidade-codigo">Código: {{ unidade.codigo }}</p>
          </div>
          <div class="actions">
            <span class="badge" [class.badge-active]="unidade.ativa" [class.badge-inactive]="!unidade.ativa">
              {{ unidade.ativa ? 'Ativa' : 'Inativa' }}
            </span>
            <button class="btn btn-warning" *ngIf="unidade.ativa" (click)="inativar(unidade)">Inativar</button>
            <button class="btn btn-success" *ngIf="!unidade.ativa" (click)="ativar(unidade)">Ativar</button>
          </div>
        </div>

        <div class="colaboradores-list" *ngIf="unidade.colaboradores && unidade.colaboradores.length > 0">
          <h4>Colaboradores:</h4>
          <ul>
            <li *ngFor="let col of unidade.colaboradores">
              {{ col.nome }} ({{ col.codigo }})
            </li>
          </ul>
        </div>
        <p class="no-colab" *ngIf="!unidade.colaboradores || unidade.colaboradores.length === 0">
          Nenhum colaborador nesta unidade.
        </p>
      </div>

      <!-- Modal Criar -->
      <div class="modal-overlay" *ngIf="showModal" (click)="showModal = false">
        <div class="modal" (click)="$event.stopPropagation()">
          <h3>Nova Unidade</h3>
          <div class="form-group">
            <label>Código</label>
            <input type="text" [(ngModel)]="novaUnidade.codigo">
          </div>
          <div class="form-group">
            <label>Nome</label>
            <input type="text" [(ngModel)]="novaUnidade.nome">
          </div>
          <div class="modal-actions">
            <button class="btn" (click)="showModal = false">Cancelar</button>
            <button class="btn btn-primary" (click)="criar()">Criar</button>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .unidade-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .unidade-header h3 {
      color: #333;
      margin-bottom: 4px;
    }

    .unidade-codigo {
      color: #666;
      font-size: 14px;
    }

    .colaboradores-list {
      margin-top: 16px;
      padding-top: 16px;
      border-top: 1px solid #eee;
    }

    .colaboradores-list h4 {
      color: #555;
      margin-bottom: 8px;
    }

    .colaboradores-list ul {
      list-style: none;
      padding: 0;
    }

    .colaboradores-list li {
      padding: 6px 0;
      color: #666;
      border-bottom: 1px solid #f0f0f0;
    }

    .no-colab {
      margin-top: 12px;
      color: #999;
      font-style: italic;
    }
  `]
})
export class UnidadesComponent implements OnInit {
  unidades: Unidade[] = [];
  showModal = false;
  novaUnidade: CriarUnidade = { codigo: '', nome: '' };
  successMsg = '';
  errorMsg = '';

  constructor(private unidadeService: UnidadeService) {}

  ngOnInit(): void {
    this.loadUnidades();
  }

  loadUnidades(): void {
    this.unidadeService.getAll().subscribe({
      next: (data) => this.unidades = data,
      error: () => this.errorMsg = 'Erro ao carregar unidades.'
    });
  }

  criar(): void {
    this.unidadeService.create(this.novaUnidade).subscribe({
      next: () => {
        this.showModal = false;
        this.novaUnidade = { codigo: '', nome: '' };
        this.successMsg = 'Unidade criada com sucesso!';
        this.loadUnidades();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao criar unidade.';
        this.clearMessages();
      }
    });
  }

  inativar(unidade: Unidade): void {
    this.unidadeService.update(unidade.id, { ativa: false }).subscribe({
      next: () => {
        this.successMsg = 'Unidade inativada com sucesso!';
        this.loadUnidades();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao inativar unidade.';
        this.clearMessages();
      }
    });
  }

  ativar(unidade: Unidade): void {
    this.unidadeService.update(unidade.id, { ativa: true }).subscribe({
      next: () => {
        this.successMsg = 'Unidade ativada com sucesso!';
        this.loadUnidades();
        this.clearMessages();
      },
      error: (err) => {
        this.errorMsg = err.error?.message || 'Erro ao ativar unidade.';
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
