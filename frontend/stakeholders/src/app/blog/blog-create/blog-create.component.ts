import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { BlogService } from '../../services/blog.service';

@Component({
  selector: 'app-blog-create',
  templateUrl: './blog-create.component.html',
  styleUrls: ['./blog-create.component.css']
})
export class BlogCreateComponent implements OnInit {
  @ViewChild('editor') editor!: ElementRef;
  
  title: string = '';
  imageUrls: string[] = [];
  isSubmitting: boolean = false;
  error: string = '';
  authorId: number = 1;

  activeStyles = { 
    bold: false, 
    italic: false, 
    underline: false, 
    strike: false, 
    h2: false, 
    ul: false 
  };

  constructor(private blogService: BlogService, private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('jwt');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.authorId = payload.id || 1;
      } catch (e) { this.authorId = 1; }
    } else {
      this.router.navigate(['/login']);
    }
  }

  execCommand(command: string, value: string = ''): void {
    document.execCommand(command, false, value);
    this.updateActiveStyles();
    this.editor.nativeElement.focus();
  }

  updateActiveStyles(): void {
    this.activeStyles.bold = document.queryCommandState('bold');
    this.activeStyles.italic = document.queryCommandState('italic');
    this.activeStyles.underline = document.queryCommandState('underline');
    this.activeStyles.strike = document.queryCommandState('strikeThrough');
    this.activeStyles.h2 = document.queryCommandValue('formatBlock') === 'h2';
    this.activeStyles.ul = document.queryCommandState('insertUnorderedList');
  }

  onFilesSelected(event: any): void {
    const files: FileList = event.target.files;
    if (files) {
      for (let i = 0; i < files.length; i++) {
        const reader = new FileReader();
        reader.onload = (e: any) => this.imageUrls.push(e.target.result);
        reader.readAsDataURL(files[i]);
      }
    }
  }

  removeImage(index: number): void {
    this.imageUrls.splice(index, 1);
  }

  submit(): void {
    const content = this.editor.nativeElement.innerHTML;
    if (!this.title.trim() || !content.trim() || content === '<br>') {
      this.error = 'Title and content are required.';
      return;
    }

    this.isSubmitting = true;
    this.blogService.create({
      title: this.title,
      description: content,
      authorId: this.authorId,
      imageUrls: this.imageUrls
    }).subscribe({
      next: () => this.router.navigate(['/home']),
      error: () => {
        this.error = 'Error publishing post.';
        this.isSubmitting = false;
      }
    });
  }

  cancel() { this.router.navigate(['/home']); }
}