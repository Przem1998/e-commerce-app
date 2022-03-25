import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrdersRoutingModule } from './orders-routing.module';
import { OrderDetailsComponent } from './order-details/order-details.component';
import { OrdersComponent } from './orders.component';
import { DashboardModule } from '../dashboard.module';
import { AdminNavBarComponent } from '../admin-nav-bar/admin-nav-bar.component';


@NgModule({
  declarations: [
    OrderDetailsComponent,
    OrdersComponent
  ],
  imports: [
    CommonModule,
    OrdersRoutingModule,
  ]
})
export class OrdersModule { }
