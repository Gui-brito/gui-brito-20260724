import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Usuario, CriarUsuario, AtualizarUsuario } from '../models/usuario.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private apiUrl = `${environment.apiUrl}/usuarios`;

  constructor(private http: HttpClient) {}

  getAll(ativo?: boolean): Observable<Usuario[]> {
    let params = new HttpParams();
    if (ativo !== undefined) {
      params = params.set('ativo', ativo.toString());
    }
    return this.http.get<Usuario[]>(this.apiUrl, { params });
  }

  create(usuario: CriarUsuario): Observable<Usuario> {
    return this.http.post<Usuario>(this.apiUrl, usuario);
  }

  update(id: number, usuario: AtualizarUsuario): Observable<Usuario> {
    return this.http.put<Usuario>(`${this.apiUrl}/${id}`, usuario);
  }
}
