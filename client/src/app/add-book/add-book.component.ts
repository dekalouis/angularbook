import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { MaterialModule } from '../material.module';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'app-add-book',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent, MaterialModule],
  templateUrl: './add-book.component.html',
  styleUrls: ['./add-book.component.css'],
})
export class AddBookComponent {
  title = '';
  author = '';
  errorMessage = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private snackbar: SnackbarService
  ) {}

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
        next: () => {
          this.snackbar.show('New book added successfully!');
          this.router.navigate(['/books']);
        },
        error: (err) => {
          this.snackbar.show('Failed to add book.', 'Dismiss', 4000);
          this.errorMessage =
            err?.error?.message || 'Failed to add book. Please try again.';
        },
      });
  }
}
