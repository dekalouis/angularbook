import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // for *ngIf
import { FormsModule } from '@angular/forms'; // for ngModel, ngForm
import { RouterModule } from '@angular/router'; // for routerLink
import { NavbarComponent } from '../navbar/navbar.component'; // adjust path if needed

@Component({
  selector: 'app-update-book',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, NavbarComponent],
  templateUrl: './update-book.component.html',
})
export class UpdateBookComponent implements OnInit {
  title: string = '';
  author: string = '';
  errorMessage = '';
  bookId!: number;

  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${localStorage.getItem('token')}`,
    }),
  };

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.bookId = +this.route.snapshot.paramMap.get('id')!;
    this.http
      .get<any>(
        `http://localhost:5058/api/book/${this.bookId}`,
        this.httpOptions
      )
      .subscribe({
        next: (book) => {
          this.title = book.title;
          this.author = book.author;
        },
        error: (err) => {
          this.errorMessage = 'Failed to load book';
          console.error(err);
        },
      });
  }

  onSubmit(): void {
    this.http
      .put(
        `http://localhost:5058/api/book/${this.bookId}`,
        {
          title: this.title,
          author: this.author,
        },
        this.httpOptions
      )
      .subscribe({
        next: () => this.router.navigate(['/books']),
        error: (err) => {
          this.errorMessage = 'Failed to update book';
          console.error(err);
        },
      });
  }
}
