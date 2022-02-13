import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../account/login/login.component';
import { AuthRoleGuard } from '../core/guards/auth-role.guard';

import { CategoriesComponent } from './categories/categories.component';
import { DashboardComponent } from './dashboard.component';
import { OrdersComponent } from './orders/orders.component';
import { ProductsComponent } from './products/products.component';

const routes: Routes = [
  {path:'admin', canActivate:[AuthRoleGuard], component:DashboardComponent},
  {path:'admin/products',component:ProductsComponent},
  {path:'admin/ordersCustomers', component: OrdersComponent},
  {path:'admin/categories',component:CategoriesComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
