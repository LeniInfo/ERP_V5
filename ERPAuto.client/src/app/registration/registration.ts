import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  FormGroup,
  AbstractControl
} from '@angular/forms';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './registration.html',
  styleUrls: ['./registration.css']
})
export class Registration {

  registerForm!: FormGroup;

  showPassword = false;
  strengthWidth = '0%';
  showSuccess = false;
  loading = false;

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group(
      {
        name: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', Validators.required]
      },
      { validators: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(group: AbstractControl) {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  onPasswordInput() {
    const value = this.registerForm.get('password')?.value || '';

    this.strengthWidth =
      value.length < 4 ? '30%' :
      value.length < 8 ? '60%' : '100%';
  }

  register() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.loading = true;

    // 🔥 simulate API call
    setTimeout(() => {
      this.loading = false;
      this.showSuccess = true;
      console.log('FORM DATA 👉', this.registerForm.value);
    }, 1500);
  }
}
