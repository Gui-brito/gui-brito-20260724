export interface Usuario {
  id: number;
  codigo: string;
  login: string;
  ativo: boolean;
}

export interface CriarUsuario {
  login: string;
  senha: string;
}

export interface AtualizarUsuario {
  senha?: string;
  ativo?: boolean;
}

export interface LoginRequest {
  login: string;
  senha: string;
}

export interface LoginResponse {
  token: string;
  usuario: Usuario;
}
