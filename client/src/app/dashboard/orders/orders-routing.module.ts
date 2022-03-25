import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrdersComponent } from './orders.component';


const routes: Routes = [
  {path:'admin/ordersCustomers',component:OrdersComponent, data:{breadcrumb: 'Zamówienia'}},
  {path:'admin/ordersCustomers/:id', component: OrderDetailsComponent, data:{breadcrumb:{alias: 'OrderDetailed'}}}];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrdersRoutingModule { }
 