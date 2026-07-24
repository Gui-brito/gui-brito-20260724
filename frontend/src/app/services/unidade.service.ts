import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Unidade, CriarUnidade, AtualizarUnidade } from '../models/unidade.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UnidadeService {
  private apiUrl = `${environment.apiUrl}/unidades`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Unidade[]> {
    return this.http.get<Unidade[]>(this.apiUrl);
  }

  create(unidade: CriarUnidade): Observable<Unidade> {
    return this.http.post<Unidade>(this.apiUrl, unidade);
  }

  update(id: number, unidade: AtualizarUnidade): Observable<Unidade> {
    return this.http.put<Unidade>(`${this.apiUrl}/${id}`, unidade);
  }
}
