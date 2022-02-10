import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { ISystemType } from '../shared/models/systemType';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = "https://localhost:5001/api/"

  constructor(private http: HttpClient) { }

  getProduct(id:number){
    return this.http.get<IProduct>(this.baseUrl+'products/'+id);
  }

  getProducts(shopParams:ShopParams){
    let params = new HttpParams();
     
    if(shopParams.systemId !== 0){
      params=params.append('systemId',shopParams.systemId.toString());
    }
    if(shopParams.typeId !== 0){
      params = params.append('typeId', shopParams.typeId.toString());
    }
    if(shopParams.search){
      params=params.append('search', shopParams.search);
    }
    
    params = params.append('sort',shopParams.sort);
    params=params.append('pageIndex',shopParams.pageNumber.toString());
    params=params.append('pageSize',shopParams.pageSize.toString());

    return this.http.get<IPagination>(this.baseUrl + 'products',{observe: 'response',params})
    .pipe(
      map(response =>{
          return response.body;
      }) // mapping to IPagination
    ); 
  }
  getSystems(){
    return this.http.get<ISystemType[]>(this.baseUrl+ 'products/systems');
  }
  getTypes(){
    return this.http.get<IType[]>(this.baseUrl+ 'products/types');
  }
}
