import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Blog, CreateBlogRequest, Comments, CreateCommentRequest, EditCommentRequest } from '../models/blog.model';

@Injectable({ providedIn: 'root' })
export class BlogService {
  private baseUrl = 'http://localhost:5048/api';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Blog[]> {
    return this.http.get<Blog[]>(`${this.baseUrl}/blogs`);
  }

  getById(id: string): Observable<Blog> {
    return this.http.get<Blog>(`${this.baseUrl}/blogs/${id}`);
  }

  create(request: CreateBlogRequest): Observable<Blog> {
    return this.http.post<Blog>(`${this.baseUrl}/blogs`, request);
  }

  getComments(blogId: string): Observable<Comments[]> {
    return this.http.get<Comments[]>(`${this.baseUrl}/blogs/${blogId}/comments`);
  }

  addComment(blogId: string, request: CreateCommentRequest): Observable<Comments> {
    return this.http.post<Comments>(`${this.baseUrl}/blogs/${blogId}/comments`, request);
  }

  editComment(commentId: string, request: EditCommentRequest): Observable<Comments> {
    return this.http.put<Comments>(`${this.baseUrl}/comments/${commentId}`, request);
  }

}