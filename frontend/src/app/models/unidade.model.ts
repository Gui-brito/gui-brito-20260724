export interface Unidade {
  id: number;
  codigo: string;
  nome: string;
  ativa: boolean;
  colaboradores: ColaboradorResumo[];
}

export interface ColaboradorResumo {
  id: number;
  codigo: string;
  nome: string;
}

export interface CriarUnidade {
  codigo: string;
  nome: string;
}

export interface AtualizarUnidade {
  ativa: boolean;
}
