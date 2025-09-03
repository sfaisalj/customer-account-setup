import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { SetupComponent } from './components/setup/setup.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'setup', component: SetupComponent },
  { path: '**', redirectTo: '/login' }
];
