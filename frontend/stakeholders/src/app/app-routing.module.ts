import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { BlogCreateComponent } from './blog/blog-create/blog-create.component';
import { BlogListComponent } from './blog/blog-list/blog-list.component';
import { BlogDetailComponent } from './blog/blog-detail/blog-detail.component';
import { ProfileComponent } from './profile/profile.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { EditProfileComponent } from './edit-profile/edit-profile.component';
import { MyToursComponent } from './my-tours/my-tours.component';
import { CreateTourComponent } from './create-tour/create-tour.component';
import { TourDetailsComponent } from './tour-details/tour-details.component';
import { TourPageComponent } from './tour-page/tour-page.component';
import { ToursComponent } from './tours/tours.component';
import { PositionSimulatorComponent } from './position-simulator/position-simulator.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'create-blog', component: BlogCreateComponent },
  { path: 'blogs', component: BlogListComponent },
  { path: 'blogs/:id', component: BlogDetailComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'profile', component: ProfileComponent },
  { path: 'user/:id', component: UserDetailsComponent },
  { path: 'edit-profile', component: EditProfileComponent },
  { path: 'my-tours', component: MyToursComponent },
  { path: 'create-tour', component: CreateTourComponent },
  { path: 'tour-details/:id', component: TourDetailsComponent },
  { path: 'tour-page/:id', component: TourPageComponent },
  { path: 'tours', component: ToursComponent },
  { path: 'position-simulator', component: PositionSimulatorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
