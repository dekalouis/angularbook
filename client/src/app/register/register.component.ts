import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, MaterialModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  email = '';
  password = '';
  confirmPassword = '';
  errorMessage = '';

  constructor(private http: HttpClient, private router: Router) {}

  onSubmit() {
    if (this.password !== this.confirmPassword) {
      this.errorMessage = 'Passwords do not match.';
      return;
    }

    // this.http
    //   .post('http://localhost:5058/api/auth/register', {
    //     email: this.email,
    //     password: this.password,
    //   })
    //   .subscribe({
    //     next: () => this.router.navigate(['/login']),
    //     // error: (err) => {
    //     //   this.errorMessage =
    //     //     err.error?.title ||
    //     //     err.error ||
    //     //     'Registration failed. Please try again.';
    //     // },
    //     error: (err) => {
    //       const backendError = err.error?.message || err.error?.title;
    //       this.errorMessage =
    //         typeof backendError === 'string'
    //           ? backendError
    //           : 'Registration failed. Please try again.';
    //     },
    //   });
    this.http
      .post('http://localhost:5058/api/auth/register', {
        email: this.email,
        password: this.password,
      })
      .subscribe({
        next: (res) => {
          //log this biar bisa dicek
          console.log('✅ SUCCESS RESPONSE:', res);
          this.router.navigate(['/login']);
        },
        error: (err) => {
          //log this biar bisa dicek
          console.log('❌ ERROR RESPONSE:', err);
          const backendError = err.error?.message || err.error?.title;
          this.errorMessage =
            typeof backendError === 'string'
              ? backendError
              : 'Registration failed. Please try again.';
        },
      });
  }
}
