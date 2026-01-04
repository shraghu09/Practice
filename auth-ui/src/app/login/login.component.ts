import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  imports: [FormsModule]
})
export class LoginComponent {

  email = '';
  password = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  login() {
    this.authService.login(this.email, this.password)
      .subscribe({
        next: (res) => {
          // ✅ save token
          localStorage.setItem('token', res.token);

          // ✅ save username
          localStorage.setItem('username', this.email);

          // ✅ redirect to homepage
          this.router.navigate(['/homepage']);
        },
        error: () => {
          alert('Invalid login');
        }
      });
  }
}
