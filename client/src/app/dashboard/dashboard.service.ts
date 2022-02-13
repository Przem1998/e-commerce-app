import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IOrder } from '../shared/models/order';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ISystemType } from '../shared/models/systemType';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  baseUrl=environment.apiUrl;

  constructor(private http: HttpClient) { }

  isHaveAdminPermission(){
    // this.currentUser$.subscribe(res=>{
    //   if(res == null) return false;
    //   if(res.role=="ADMIN") return true;
    //   else return false;
    // }, error =>{
    //   console.log(error);
    // })
    
  }

 getAllOrders(){
   return this.http.get(this.baseUrl+'order/allOrders');
 }
 getAllStatus(){
  return this.http.get(this.baseUrl+'admin/allStatus');
}
 changeOrderStatus(order:IOrder){
   return this.http.put<IOrder>(this.baseUrl+'order/changeStatus',order);
 }
 getSystems(){
  return this.http.get<ISystemType[]>(this.baseUrl+ 'products/systems');
}
getTypes(){
  return this.http.get<IType[]>(this.baseUrl+ 'products/types');
}
checkProductExists(product: string) {
 return this.http.get<boolean>(this.baseUrl+'admin/productExists?product='+product);
}
addProduct(product:IProduct){
  return this.http.post(this.baseUrl+'admin/addProduct',product);
}
removeProduct(product:IProduct){
  return this.http.delete(this.baseUrl+'admin/deleteProduct?productId='+product.id);
}

deployImage(image:any){
  return this.http.post('https://localhost:5001/api/admin/deployImage',image)
}

}
