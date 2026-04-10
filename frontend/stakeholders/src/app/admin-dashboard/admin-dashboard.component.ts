import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';
import { User } from '../models/user.model';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  users: User[] = [];
  activeUsers: User[] = [];
  blockedUsers: User[] = [];
  activeTab: 'active' | 'blocked' = 'active';
  private currentUserId: number | null = null;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe({
      next: (res: any) => {
        if (!res || !res.role || res.role.name !== 'ROLE_ADMIN') {
          this.router.navigate(['/']);
          return;
        }

        this.currentUserId = res.id;
        this.loadUsers();
      },
      error: () => {
        this.router.navigate(['/']);
      }
    });
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (res) => {
        this.users = (res || []).filter(u => u.id !== this.currentUserId);
        this.activeUsers = this.users.filter(u => !u.blocked);
        this.blockedUsers = this.users.filter(u => u.blocked);
      },
      error: (err) => {
        console.error('Failed to load users', err);
      }
    });
  }

  toggleTab(tab: 'active' | 'blocked') {
    this.activeTab = tab;
  }

  changeBlock(user: User, block: boolean) {
    this.userService.blockUser(user.id, block).subscribe({
      next: () => {
        user.blocked = block;
        this.activeUsers = this.users.filter(u => !u.blocked);
        this.blockedUsers = this.users.filter(u => u.blocked);
      },
      error: (err) => {
        console.error('Failed to change block status', err);
      }
    });
  }

  openUser(user: User) {
    this.router.navigate([`/user/${user.id}`]);
  }
}
