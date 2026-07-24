import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Colaborador, CriarColaborador, AtualizarColaborador } from '../models/colaborador.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ColaboradorService {
  private apiUrl = `${environment.apiUrl}/colaboradores`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Colaborador[]> {
    return this.http.get<Colaborador[]>(this.apiUrl);
  }

  create(colaborador: CriarColaborador): Observable<Colaborador> {
    return this.http.post<Colaborador>(this.apiUrl, colaborador);
  }

  update(id: number, colaborador: AtualizarColaborador): Observable<Colaborador> {
    return this.http.put<Colaborador>(`${this.apiUrl}/${id}`, colaborador);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
