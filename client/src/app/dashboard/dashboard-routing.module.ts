import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../account/login/login.component';
import { CustomersComponent } from './customers/customers.component';
import { DashboardComponent } from './dashboard.component';
import { OrdersComponent } from './orders/orders.component';
import { AddProductComponent } from './products/add-product/add-product.component';
import { ProductsComponent } from './products/products.component';

const routes: Routes = [
  {path:'', component:DashboardComponent},
  {path:'logowanie', component:LoginComponent},
  {path:'Produkty', component: ProductsComponent},
  {path:'Zamówienia', component: OrdersComponent},
  {path:'Klienci',component:CustomersComponent},
  {path:'Dodaj',component:AddProductComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
