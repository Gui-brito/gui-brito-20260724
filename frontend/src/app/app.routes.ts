import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'dashboard',
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'usuarios', pathMatch: 'full' },
      {
        path: 'usuarios',
        loadComponent: () => import('./pages/usuarios/usuarios.component').then(m => m.UsuariosComponent)
      },
      {
        path: 'colaboradores',
        loadComponent: () => import('./pages/colaboradores/colaboradores.component').then(m => m.ColaboradoresComponent)
      },
      {
        path: 'unidades',
        loadComponent: () => import('./pages/unidades/unidades.component').then(m => m.UnidadesComponent)
      }
    ]
  },
  { path: '**', redirectTo: '/login' }
];
