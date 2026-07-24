export interface Colaborador {
  id: number;
  codigo: string;
  nome: string;
  unidadeNome: string;
  unidadeId: number;
  usuarioId: number;
  usuarioLogin: string;
}

export interface CriarColaborador {
  nome: string;
  unidadeId: number;
  usuarioId: number;
}

export interface AtualizarColaborador {
  nome?: string;
  unidadeId?: number;
}
