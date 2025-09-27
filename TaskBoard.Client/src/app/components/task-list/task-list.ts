import { Component, signal } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { TaskItem, TaskStatus } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  imports: [],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskList {
  tasks = signal<TaskItem[]>([]);

  constructor(private taskService: TaskService) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getAll().subscribe({
      next: (data) => this.tasks.set(data),
      error: (error) => console.error(error),
    });
  }

  //helper for status
  getStatusLabel(status: TaskStatus) {
    switch (status) {
      case TaskStatus.Todo:
        return 'Todo';
      case TaskStatus.InProgress:
        return 'In Progress';
      case TaskStatus.Done:
        return 'Done';
      default:
        return '';
    }
  }

  deleteTask(id: number) {
    this.taskService.delete(id).subscribe({
      next: () => this.loadTasks(),
      error: (err) => console.error(err),
    });
  }

  toggleTaskStatus(task: TaskItem) {
    this.taskService.toggleStatus(task).subscribe({
      next: () => this.loadTasks(),
      error: (err) => console.error(err),
    });
  }
}
