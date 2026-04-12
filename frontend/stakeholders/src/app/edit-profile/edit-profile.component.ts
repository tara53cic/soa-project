import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  editForm: FormGroup;
  private readonly baseUrl = 'http://localhost:8080/api/users';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.editForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      motto: [''],
      biography: [''],
      profilePicture: ['']
    });
  }

  ngOnInit(): void {
    this.loadUserData();
  }

  private loadUserData(): void {
    const token = localStorage.getItem('jwt');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    this.http.get(`${this.baseUrl}/whoami`, { headers }).subscribe({
      next: (user: any) => {
        this.editForm.patchValue({
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email,
          motto: user.motto,
          biography: user.biography,
          profilePicture: user.profilePicture
        });
      },
      error: (err: any) => {
        console.error('Could not load user data', err);
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.editForm.patchValue({ profilePicture: reader.result as string });
      };
      reader.readAsDataURL(file);
    }
  }

  save(): void {
    if (this.editForm.valid) {
      const token = localStorage.getItem('jwt');
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

      const updateData = {
        firstName: this.editForm.value.firstName,
        lastName: this.editForm.value.lastName,
        email: this.editForm.value.email,
        motto: this.editForm.value.motto || "",
        biography: this.editForm.value.biography || "",
        profilePicture: this.editForm.value.profilePicture || null
      };

      this.http.put(`${this.baseUrl}/profile`, updateData, { headers }).subscribe({
        next: () => {
          this.router.navigate(['/profile']).then(() => {
            window.location.reload();
          });
        },
        error: (err: any) => {
          console.error('Update failed', err);
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/profile']);
  }
}