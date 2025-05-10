import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { BookListComponent } from './book-list/book-list.component';
import { AddBookComponent } from './add-book/add-book.component';
import { RegisterComponent } from './register/register.component';
import { UpdateBookComponent } from './update-book/update-book.component';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'books', component: BookListComponent },
  { path: 'add-book', component: AddBookComponent },
  { path: 'book/edit/:id', component: UpdateBookComponent },
];
