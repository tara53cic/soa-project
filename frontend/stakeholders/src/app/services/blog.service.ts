import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Blog, CreateBlogRequest } from '../models/blog.model';

@Injectable({ providedIn: 'root' })
export class BlogService {
  private baseUrl = 'http://localhost:5002/api';

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
}