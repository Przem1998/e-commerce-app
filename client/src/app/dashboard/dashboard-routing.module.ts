import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../account/login/login.component';
import { AuthRoleGuard } from '../core/guards/auth-role.guard';

import { CategoriesComponent } from './categories/categories.component';
import { DashboardComponent } from './dashboard.component';

const routes: Routes = [
  {path:'admin', canActivate:[AuthRoleGuard], component:DashboardComponent, data:{breadcrumb: 'Panel administratora'}},
  {path:'admin/products',loadChildren: () => import('./products/products.module').then(mod => mod.ProductsModule), data:{breadcrumb: 'Produkty'}},
  {path:'admin/ordersCustomers',loadChildren: () => import('./orders/orders.module').then(mod => mod.OrdersModule), data:{breadcrumb: 'Zamówienia'}},
  {path:'admin/categories',component:CategoriesComponent, data:{breadcrumb: 'Kategorie'}},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
