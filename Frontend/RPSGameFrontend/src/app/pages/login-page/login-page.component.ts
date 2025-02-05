  import { Component, inject } from '@angular/core';
  import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
  import { AuthService } from '../../services/auth-service.service';
  import { AuthRequest } from '../../cores/models/auth/AuthRequest';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';

  @Component({
    selector: 'app-login-page',
    standalone: true,
    imports: [ReactiveFormsModule, CommonModule, RouterLink],
    templateUrl: './login-page.component.html',
    styleUrls: ['./login-page.component.scss']
  })
  export class LoginPageComponent {
    authService = inject(AuthService);
    router = inject(Router);

    form: FormGroup = new FormGroup({
      email: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required])
    });

    onSubmit() {
      if (this.form.valid) {
        const authRequest: AuthRequest = {
          email: this.form.value.email,
          password: this.form.value.password
        };

        this.authService.login(authRequest).subscribe({
          next: (response) => {
            this.router.navigate([''])
            console.log('Login successful:', response);
          },
          error: (err) => {
            console.error('Login failed:', err);
          }
        });
      }
    }
  }
