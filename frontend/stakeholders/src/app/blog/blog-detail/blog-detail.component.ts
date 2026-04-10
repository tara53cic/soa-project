import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Blog } from '../../models/blog.model';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-blog-detail',
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.css']
})
export class BlogDetailComponent implements OnInit {
  blog?: Blog;
  isLoading = true;
  error = '';
  currentImageIndex = 0;
  isGalleryOpen = false;

  constructor(
    private route: ActivatedRoute,
    private blogService: BlogService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.blogService.getById(id).subscribe({
        next: (data) => {
          this.blog = data;
          this.isLoading = false;
        },
        error: () => {
          this.error = 'Blog post not found.';
          this.isLoading = false;
        }
      });
    }
  }

  nextImage(event?: Event): void {
    if (event) event.stopPropagation();
    if (this.blog?.imageUrls && this.blog.imageUrls.length > 1) {
      this.currentImageIndex = (this.currentImageIndex + 1) % this.blog.imageUrls.length;
    }
  }

  prevImage(event?: Event): void {
    if (event) event.stopPropagation();
    if (this.blog?.imageUrls && this.blog.imageUrls.length > 1) {
      this.currentImageIndex = (this.currentImageIndex - 1 + this.blog.imageUrls.length) % this.blog.imageUrls.length;
    }
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (this.isGalleryOpen) {
      if (event.key === 'ArrowRight') this.nextImage();
      if (event.key === 'ArrowLeft') this.prevImage();
      if (event.key === 'Escape') this.closeGallery();
    }
  }

  openGallery(): void {
    if (this.blog?.imageUrls && this.blog.imageUrls.length > 0) {
      this.isGalleryOpen = true;
      document.body.style.overflow = 'hidden';
    }
  }

  closeGallery(): void {
    this.isGalleryOpen = false;
    document.body.style.overflow = 'auto';
  }

  goBack(): void {
    this.router.navigate(['/blogs']);
  }

  getInitials(username: string): string {
    return username ? username.substring(0, 2).toUpperCase() : 'AU';
  }
}