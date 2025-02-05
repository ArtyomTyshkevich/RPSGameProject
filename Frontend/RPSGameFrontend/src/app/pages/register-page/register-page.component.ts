import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth-service.service';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { RegisterRequest } from '../../cores/models/auth/RegisterRequest';

@Component({
  selector: 'app-register-page',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
 authService = inject(AuthService);
 router = inject(Router);
 
 form: FormGroup = new FormGroup({
  nickname: new FormControl('', [Validators.required]),
  email: new FormControl('', [Validators.required]),
  password: new FormControl('', [Validators.required]),
  passwordConfirm: new FormControl('', [Validators.required]),
  birthDate: new FormControl('', [Validators.required]) 
});

onSubmit() {
  if (this.form.valid) {
    const registerRequest: RegisterRequest = {
      nickname: this.form.value.nickname,
      email: this.form.value.email,
      password: this.form.value.password,
      passwordConfirm: this.form.value.passwordConfirm,
      birthDate: this.form.value.birthDate 
    };

    this.authService.register(registerRequest).subscribe({
      next: (response) => {
        this.router.navigate(['']);
        console.log('Register successful:', response);
      },
      error: (err) => {
        console.error('Register failed:', err);
      }
    });
  }
}
}
