import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Blog } from '../../models/blog.model';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
  styleUrls: ['./blog-list.component.css']
})
export class BlogListComponent implements OnInit {
  @Input() limit: number = 0;
  
  blogs: Blog[] = [];
  displayBlogs: Blog[] = [];
  isLoading = true;
  error = '';

  constructor(private blogService: BlogService, private router: Router) {}

  ngOnInit(): void {
    this.blogService.getAll().subscribe({
      next: (data) => { 
        this.blogs = data;
        this.displayBlogs = (this.limit > 0) ? this.blogs.slice(0, this.limit) : this.blogs;
        this.isLoading = false; 
      },
      error: () => { 
        this.error = 'Error loading blogs.'; 
        this.isLoading = false; 
      }
    });
  }

  goToSeeAll(): void {
    this.router.navigate(['/blogs']);
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('en-US', {
      day: 'numeric', month: 'short', year: 'numeric'
    });
  }

  getInitials(username: string): string {
    return username ? username.substring(0, 2).toUpperCase() : 'AU';
  }

  goBack(): void {
    this.router.navigate(['/home']);
  }

  goToBlog(id: string): void {
    this.router.navigate(['/blogs', id]);
  }
}