import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { BookListComponent } from './book-list/book-list.component';
import { AddBookComponent } from './add-book/add-book.component';
import { RegisterComponent } from './register/register.component';
import { UpdateBookComponent } from './update-book/update-book.component';
import { authRedirectGuard } from './guards/auth-redirect.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [authRedirectGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [authRedirectGuard],
  },
  { path: 'books', component: BookListComponent },
  { path: 'add-book', component: AddBookComponent },
  { path: 'book/edit/:id', component: UpdateBookComponent },
];
