import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ISize } from '../shared/models/size';
import { IPagination } from '../shared/models/pagination';
import { IType } from '../shared/models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';

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
     
    if(shopParams.sizeId !== 0){
      params=params.append('sizeId',shopParams.sizeId.toString());
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
  getSizes(){
    return this.http.get<ISize[]>(this.baseUrl+ 'products/sizes');
  }
  getTypes(){
    return this.http.get<IType[]>(this.baseUrl+ 'products/types');
  }
}
