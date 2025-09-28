import { Injectable, signal } from '@angular/core';
import { TaskItem, TaskItemDto, TaskStatus } from '../models/task.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private readonly _base = '/tasks';

  constructor(private http: HttpClient) {}

  //methods
  getAll(): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(this._base);
  }

  getById(id: number): Observable<TaskItem> {
    return this.http.get<TaskItem>(`${this._base}/${id}`);
  }

  create(dto: TaskItemDto): Observable<TaskItem> {
    return this.http.post<TaskItem>(this._base, dto);
  }

  update(id: number, dto: TaskItemDto): Observable<TaskItem> {
    return this.http.put<TaskItem>(`${this._base}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this._base}/${id}`);
  }

  toggleStatus(task: TaskItem): Observable<TaskItem> {
    let newStatus: TaskStatus;

    switch (task.status) {
      case TaskStatus.Todo:
        newStatus = TaskStatus.InProgress;
        break;
      case TaskStatus.InProgress:
        newStatus = TaskStatus.Done;
        break;
      case TaskStatus.Done:
      default:
        newStatus = TaskStatus.Todo;
    }

    const dto = { ...task, status: newStatus };
    return this.http.put<TaskItem>(`${this._base}/${task.id}`, dto);
  }
}
