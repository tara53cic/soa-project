import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Blog, Comments, CreateCommentRequest,  EditCommentRequest } from '../../models/blog.model';
import { BlogService} from '../../services/blog.service';
import { FormsModule } from '@angular/forms';

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

  comments: Comments[] = [];
  newComment = '';
  editingCommentId: string | null = null;
  editedCommentText = '';
  commentsLoading = false;
  commentsError = '';
  authorUsername: string = '';

  constructor(
    private route: ActivatedRoute,
    private blogService: BlogService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const token = localStorage.getItem('jwt');

    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.authorUsername =
          payload.sub ||
          payload.id ||
          payload.unique_name ||
          payload.nameid ||
          '';

        console.log('Logged in as:', this.authorUsername);

        if (!this.authorUsername) {
          this.error = 'User identification failed. Please log in again.';
        }
      } catch (e) {
        console.error('Token error:', e);
        this.router.navigate(['/login']);
        return;
      }
    } else {
      this.router.navigate(['/login']);
      return;
    }

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

    this.blogService.getById(id!).subscribe({
      next: (data) => {
        this.blog = data;
        this.isLoading = false;
        this.loadComments(id!);
      },
      error: () => {
        this.error = 'Blog post not found.';
        this.isLoading = false;
      }
    });
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


  loadComments(blogId: string): void {
    this.commentsLoading = true;
    this.commentsError = '';

    this.blogService.getComments(blogId).subscribe({
      next: (data) => {
        this.comments = data;
        this.commentsLoading = false;
      },
      error: (err) => {
        console.error(err);
        this.commentsError = 'Failed to load comments.';
        this.commentsLoading = false;
      }
    });
  }

  addComment(): void {
    if (!this.blog?.id) return;

    const text = this.newComment.trim();
    if (!text) return;

    this.blogService.addComment(this.blog.id, {
      text,
      authorUsername: this.authorUsername
    }).subscribe({
      next: (comment) => {
        this.comments.push(comment);
        this.newComment = '';
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
  
  startEditComment(comment: Comments): void {
    this.editingCommentId = comment.id;
    this.editedCommentText = comment.text;
  }

  cancelEditComment(): void {
    this.editingCommentId = null;
    this.editedCommentText = '';
  }

  saveCommentEdit(commentId: string): void {
    const text = this.editedCommentText.trim();
      if (!text) return;

    this.blogService.editComment(commentId, {text}).subscribe({
      next: (updatedComment) => {
        const index = this.comments.findIndex(c => c.id === commentId);

        if (index !== -1) {
          this.comments[index] = updatedComment;
        }

        this.editingCommentId = null;
        this.editedCommentText = '';
      },
      error: (err) => {
        console.error(err);
      }
    });
  }
}