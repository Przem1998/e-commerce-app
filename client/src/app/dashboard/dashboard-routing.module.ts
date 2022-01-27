import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from '../account/login/login.component';
import { CustomersComponent } from './customers/customers.component';
import { DashboardComponent } from './dashboard.component';
import { OrdersComponent } from './orders/orders.component';
import { AddProductComponent } from './products/add-product/add-product.component';
import { ProductsComponent } from './products/products.component';

const routes: Routes = [
  {path:'admin', component:DashboardComponent},
  {path:'logowanie', component:LoginComponent},
  {path:'Produkty', 
        children:[
          {path:'', component:ProductsComponent},
          {path:'Dodaj', component: AddProductComponent}
        ]},
  {path:'Zamówienia', component: OrdersComponent},
  {path:'Klienci',component:CustomersComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DashboardRoutingModule { }
