import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { ApiService, LoginRequest } from '../../services/api.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(
    private api: ApiService,
    private router: Router
  ) { }

  loginData = {
    userId: '',
    password: '',
    rememberMe: false,
    errorMsg: ''
  };

onLogin() {

  const payload = {
    userId: this.loginData.userId,
    password: this.loginData.password
  };

  this.api.login(payload).subscribe({
    next: (res) => {

      if (res.flag === '1') {

        sessionStorage.setItem('fran', res.fran);

        Swal.fire({
          icon: 'success',
          title: 'Login Successful',
          text: 'Welcome to ERP',
          timer: 1500,
          showConfirmButton: false
        }).then(() => {
          // Navigate to default page after login (redirects to customer list)
          this.router.navigate(['/']);
        }); 

      } else {
        Swal.fire({
          icon: 'error',
          title: 'Login Failed',
          text: 'Invalid UserId or Password'
        });
      }
    },

    error: (err) => {
      Swal.fire({
        icon: 'error',
        title: 'Login Failed',
        text: err.error?.message || 'Invalid UserId or Password'
      });
    }
  });
}
}
