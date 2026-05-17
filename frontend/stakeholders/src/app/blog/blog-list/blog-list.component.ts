import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Blog } from '../../models/blog.model';
import { BlogService } from '../../services/blog.service';
import { jwtDecode } from 'jwt-decode';

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
    const token = localStorage.getItem('jwt');

    if (!token) {
      this.error = 'User not logged in.';
      this.isLoading = false;
      return;
    }

    const decodedToken: any = jwtDecode(token);
    console.log(decodedToken);

    const username =
      decodedToken.username ||
      decodedToken.unique_name ||
      decodedToken.sub;

    this.blogService.getFeed(username).subscribe({
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

  loadBlogsFeed(): void {
    const token = localStorage.getItem('jwt');

    if (!token) return;

    const decodedToken: any = jwtDecode(token);
    const username = decodedToken.username || decodedToken.unique_name || decodedToken.sub;

    this.blogService.getFeed(username).subscribe({
      next: (blogs) => {
        this.blogs = blogs;
        this.displayBlogs = this.limit > 0 ? blogs.slice(0, this.limit) : blogs;
      }
    });
  }
}