import { Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { OrdersComponent } from './pages/orders/orders.component';
import { ProductsComponent } from './pages/products/products.component';
import { CustomersComponent } from './pages/customers/customers.component';
import { StoragesComponent } from './pages/storages/storages.component';
import { UserClaimsComponent } from './pages/user-claims/user-claims.component';
import { UserClaimDetailComponent } from './pages/user-claim-detail/user-claim-detail.component';
import { ProductDetailComponent } from './pages/product-detail/product-detail.component';
import { HomeComponent } from './pages/home/home.component';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { StorageDetailComponent } from './pages/storage-detail/storage-detail.component';
import { CustomerDetailComponent } from './pages/customer-detail/customer-detail.component';
import { LoginGuard } from './core/guards/login-guard.guard';

export const routes: Routes = [
  { path:'', redirectTo:'/login', pathMatch: 'full'},
  { path: 'home', component: HomeComponent, canActivate: [LoginGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'orders', component: OrdersComponent , canActivate: [LoginGuard]},
  { path: 'products', component: ProductsComponent , canActivate: [LoginGuard]},
  { path: 'customers', component: CustomersComponent , canActivate: [LoginGuard]},
  { path: 'customer-detail/:id', component: CustomerDetailComponent , canActivate: [LoginGuard]},
  { path: 'storages', component: StoragesComponent , canActivate: [LoginGuard]},
  { path: 'storage-detail/:id', component: StorageDetailComponent, canActivate: [LoginGuard]},
  { path: 'user-claims', component: UserClaimsComponent, canActivate: [LoginGuard] },
  { path: 'user-claims-detail/:id', component: UserClaimDetailComponent , canActivate: [LoginGuard]},
  { path: 'product-detail/:id', component: ProductDetailComponent, canActivate: [LoginGuard] },
  { path: 'not-found', component: NotFoundComponent , canActivate: [LoginGuard]},
  { path: '**', redirectTo:'/not-found', pathMatch:'full' }
];
