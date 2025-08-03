import Swal from 'sweetalert2'

export function useSwal() {
  const showAlert = async (options) => {
    return await Swal.fire(options)
  }

  const showInfo = async (message) => {
    return await showAlert({
      position: 'top-end',
      title: message,
      showConfirmButton: false,
      timer: 1000,
    })
  }

  const showSuccess = async (message) => {
    return await showAlert({
      position: 'top-end',
      icon: 'success',
      title: message,
      showConfirmButton: false,
      timer: 1000,
    })
  }

  const showError = async (message) => {
    return await showAlert({
      position: 'center',
      icon: 'error',
      title: message,
      showConfirmButton: false,
      timer: 1000,
    })
  }

  const showConfirm = async (title, message) => {
    return await showAlert({
      title: title,
      text: message,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sure',
    })
  }

  return { showError, showSuccess, showConfirm, showInfo }
}