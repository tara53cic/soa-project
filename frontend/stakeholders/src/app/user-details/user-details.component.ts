import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { User } from '../models/user.model';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  user: User | null = null;
  loading = true;
  error: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (!id) {
      this.router.navigate(['/']);
      return;
    }

    this.userService.getUserById(id).subscribe({
      next: (u) => { this.user = u; this.loading = false; },
      error: (err) => { this.error = 'Unable to load user details.'; this.loading = false; console.error(err); }
    });
  }

  toggleBlock() {
    if (!this.user) return;
    const block = !this.user.blocked;
    this.userService.blockUser(this.user.id, block).subscribe({
      next: () => { if (this.user) this.user.blocked = block; },
      error: (err) => { console.error('Failed to change block status', err); }
    });
  }
}
