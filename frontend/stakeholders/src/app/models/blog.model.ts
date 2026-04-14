export interface Blog {
  id: string;
  title: string;
  description: string;
  descriptionHtml: string;
  createdAt: string;
  authorUsername: string;
  imageUrls: string[];
  comments: Comments[];
  likesCount: number;
  isLikedByCurrentUser: boolean;
}

export interface CreateBlogRequest {
  title: string;
  description: string;
  authorUsername: string;
  imageUrls: string[];
}

export interface Comments {
  id: string;
  text: string;
  authorUsername: string;
  createdAt: string;
  editedAt?: string;
}

export interface CreateCommentRequest {
  text: string;
  authorUsername: string;
}

export interface EditCommentRequest {
  text: string;
}

