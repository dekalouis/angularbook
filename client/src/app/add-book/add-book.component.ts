import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent],
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css'],
})
export class AddBookComponent {
  title = '';
  author = '';
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    this.http
      .post(
        'http://localhost:5058/api/book',
        { title: this.title, author: this.author },
        { headers }
      )
      .subscribe({
        next: () => this.router.navigate(['/books']),
        error: (err) => {
          const backendError =
            err.error?.message ||
            (typeof err.error === 'string' ? err.error : null);

          this.errorMessage =
            backendError || 'Failed to add book. Please try again.';
        },
      });
  }
}
