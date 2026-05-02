import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './navbar/navbar.component';
import { BlogListComponent } from './blog/blog-list/blog-list.component';
import { BlogCreateComponent } from './blog/blog-create/blog-create.component';
import { BlogDetailComponent } from './blog/blog-detail/blog-detail.component';
import { ProfileComponent } from './profile/profile.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { AdminDashboardComponent } from './admin-dashboard/admin-dashboard.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MyToursComponent } from './my-tours/my-tours.component';
import { CreateTourComponent } from './create-tour/create-tour.component';
import { TourDetailsComponent } from './tour-details/tour-details.component';
import { TourPageComponent } from './tour-page/tour-page.component';
import { ToursComponent } from './tours/tours.component';
import { ShoppingCartComponent } from './shopping-cart/shopping-cart.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    NavbarComponent,
    BlogListComponent,
    BlogCreateComponent,
    BlogDetailComponent,
    ProfileComponent,
    AdminDashboardComponent,
    UserDetailsComponent,
    EditProfileComponent,
    MyToursComponent,
    CreateTourComponent,
    TourDetailsComponent,
    TourPageComponent,
    ToursComponent,
    ShoppingCartComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
