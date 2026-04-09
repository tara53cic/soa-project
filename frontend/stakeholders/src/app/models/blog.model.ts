export interface Blog {
  id: string;
  title: string;
  description: string;
  descriptionHtml: string;
  createdAt: string;
  authorUsername: string;
  imageUrls: string[];
}

export interface CreateBlogRequest {
  title: string;
  description: string;
  authorUsername: string;
  imageUrls: string[];
}