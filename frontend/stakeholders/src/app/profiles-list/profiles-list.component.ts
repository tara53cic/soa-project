import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FollowService } from '../services/follow.service';
import { Router } from '@angular/router';

interface ProfileFollowStatus {
  username: string;
  following: boolean;
}

@Component({
  selector: 'app-profiles-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './profiles-list.component.html',
  styleUrls: ['./profiles-list.component.css']
})
export class ProfilesListComponent implements OnInit {
  profiles: ProfileFollowStatus[] = [];
  loggedUsername: string = '';

  constructor(private followService: FollowService, private router: Router) {}

  ngOnInit(): void {
    this.loggedUsername = localStorage.getItem('username') || '';
    this.loadProfiles();
  }

  loadProfiles(): void {
    this.followService
      .getAllProfilesWithFollowStatus()
      .subscribe({
        next: (data) => {
          this.profiles = data;
        },
        error: (err) => {
          console.error('Error loading profiles:', err);
        }
      });
  }

  follow(profile: ProfileFollowStatus): void {

    this.followService
      .follow(profile.username)
      .subscribe({
        next: () => {
          profile.following = true;
        },
      error: (err) => {
        console.error('Error following user:', err);
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }

  getInitials(username: string): string {
    return username ? username.substring(0, 2).toUpperCase() : 'AU';
  }
}
